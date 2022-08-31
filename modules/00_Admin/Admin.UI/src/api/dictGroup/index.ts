import { crud } from 'mkh-ui'
import http from '../http';

const urls = {
  SELECT: 'DictGroup/Select',
}
export default  {
  ...crud(http,'DictGroup'),
    select: () => http.get(urls.SELECT),
  }
