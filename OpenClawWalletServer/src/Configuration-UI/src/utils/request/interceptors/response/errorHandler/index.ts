import type { AxiosError } from 'axios'
import httpErrorHandler from './http'
import bizHandler from './biz'
import { notification } from 'antd'

export function errorHandler(error: AxiosError<API.Response>) {
  const { skipErrorHandler = false, silent = false } = error.config!
  if (skipErrorHandler)
    throw error

  const errorMessage = [httpErrorHandler, bizHandler].reduce<
    string | undefined
  >((msg, handler) => msg || handler(error), '')
  // const statusCode = error?.response?.status
  if (!silent || (silent && (error.response?.data.errorData?.length ?? 0) < 1)) {
    notification.error({
      message: errorMessage || 'Request Failed',
    })
  }

  throw error
}
