import type { AxiosRequestConfig, AxiosResponse } from 'axios'
import axios from 'axios'
// import { locales, authorization } from './interceptors/request'
import { bizHandler, errorHandler } from './interceptors/response'

const instance = axios.create({
  baseURL: '/',
  timeout: 0,
  paramsSerializer: {
    indexes: null,
  },
})

// instance.interceptors.request.use(apiHost)
// instance.interceptors.request.use(locales)
// instance.interceptors.request.use(authorization)

instance.interceptors.response.use(bizHandler)
instance.interceptors.response.use(r => r, errorHandler)

export type RequestConfig = AxiosRequestConfig & {
  // 获取完整的axios响应，否则只返回data
  getWholeResponse?: boolean
  // 获取完成的业务数据，否则只返回业务数据的data
  getWholeBizData?: boolean
}

type ConfigWithWholeResponse = AxiosRequestConfig & {
  getWholeResponse: true
}

type ConfigWithWholeBizData = AxiosRequestConfig & {
  getWholeBizData: true
}

type ConfigWithOriginData = AxiosRequestConfig & {
  getWholeResponse: true
  getWholeBizData: true
}

async function request<T = unknown, O = ConfigWithOriginData>(url: string, config: O): Promise<AxiosResponse<API.Response<T>>>
async function request<T = unknown, O = ConfigWithWholeResponse>(url: string, config: O): Promise<AxiosResponse<T>>
async function request<T = unknown, O = ConfigWithWholeBizData>(url: string, config: O): Promise<API.Response<T>>
async function request<T = unknown, O = RequestConfig>(url: string, config: O): Promise<T>
async function request(url: string, config: RequestConfig) {
  const response = await instance(url, config)

  const bizDataOnly = config.getWholeBizData !== true
  if (bizDataOnly)
    response.data = response.data.data

  const getResponse = config.getWholeResponse === true
  return getResponse ? response : response.data
}

export default request
