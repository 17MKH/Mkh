import path from 'path'
import fse from 'fs-extra'

// 递归将d.ts文件中的@/types替换成相对路径
const patch = (rootDir: string, relativePath: string) => {
  fse.readdir(rootDir, (err, dirs) => {
    dirs.forEach((m) => {
      const filePath = path.resolve(rootDir, m)
      fse.stat(filePath, (err, stat) => {
        if (stat.isFile()) {
          fse.readFile(filePath, (err, data) => {
            fse.writeFile(filePath, data.toString().replace('@/types', (relativePath || './') + 'types'))
          })
        } else if (stat.isDirectory()) {
          patch(filePath, relativePath + '../')
        }
      })
    })
  })
}

patch(path.resolve('./.temp'), '')
