import type { AxiosResponse } from 'axios'
import { AxiosError } from 'axios'

const SUCCESS_CODE = 200

export function bizHandler(response: AxiosResponse): AxiosResponse {
  const { responseType = 'json' } = response.config
  if (responseType !== 'json')
    return response

  const { config, data, request } = response
  const { useBiz = true, throwIfBizDataIsFalse = true } = config

  if (useBiz) {
    const bizCodeError = data.code !== SUCCESS_CODE
    const bizFailed = throwIfBizDataIsFalse && data.data === false

    if (bizCodeError || bizFailed)
      throw new AxiosError(bizCodeError ? '业务错误' : '业务失败', data.code, config, request, response)
  }

  return response
}
