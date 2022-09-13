import { createHttp, ModuleDefinition } from 'mkh-ui'
import options from '@/options'

let http

export const useHttp = function (mod: ModuleDefinition) {
  http = createHttp(options, mod)
}
export default http
