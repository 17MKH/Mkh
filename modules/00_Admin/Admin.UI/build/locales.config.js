const fs = require('fs')
const fse = require('fs-extra')
const path = require('path')

let rootDir = process.cwd()

const createConfig = (input, fileName) => ({
  input,
  output: [
    {
      file: path.resolve(rootDir, `lib/locales/${fileName}/index.js`),
      format: 'es',
    },
  ],
})
const pkg = JSON.parse(fs.readFileSync(path.resolve(rootDir, 'package.json')))

let localePath = ''
if (pkg.name === 'mkh-ui') {
  localePath = path.resolve(rootDir, 'package/locales/lang')
} else {
  localePath = path.resolve(rootDir, 'src/locales')
}
export default fs.readdirSync(localePath).map(m => {
  return createConfig(path.resolve(localePath, m, 'index.js'), m)
})
