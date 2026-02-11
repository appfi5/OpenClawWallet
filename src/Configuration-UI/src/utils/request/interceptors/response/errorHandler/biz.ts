import type { AxiosError } from 'axios'

function bizErrorHandler(error: AxiosError<API.Response>) {
  const { response } = error

  return response?.data?.message
}

export default bizErrorHandler
