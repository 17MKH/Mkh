import typescript from 'rollup-plugin-typescript2'
import fs from 'fs'
import path from 'path'

let rootDir = process.cwd()

const createConfig = (input, fileName) => ({
  input,
  output: [
    {
      file: path.resolve(rootDir, `lib/locales/${fileName}/index.js`),
      format: 'es',
    },
  ],
  plugins: [typescript({ tsconfigOverride: { compilerOptions: { declaration: false }, include: [input] } })],
})

let localePath = path.resolve(rootDir, 'src/locales/lang')

export default fs.readdirSync(localePath).map((m) => {
  return createConfig(path.resolve(localePath, m, 'index.ts'), m)
})
