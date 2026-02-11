import 'axios'

declare module 'axios' {
  export interface AxiosRequestConfig {
    // 使用authToken
    useAuth?: boolean
    // 拦截业务错误
    useBiz?: boolean
    // 跳过错误处理
    skipErrorHandler?: boolean
    // 静默
    silent?: boolean
    // 业务数据返回false时认为失败
    throwIfBizDataIsFalse?: boolean
    // 使用apiHost
    useApiHost?: boolean
  }
}
