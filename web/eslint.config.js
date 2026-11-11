import js from '@eslint/js'
import eslintConfigPrettier from 'eslint-config-prettier'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import globals from 'globals'
import tseslint from 'typescript-eslint'
import { defineConfig, globalIgnores } from 'eslint/config'

export default defineConfig([
  globalIgnores(['dist', 'src/generated']),
  {
    files: ['**/*.{ts,tsx}'],
    extends: [
      js.configs.recommended,
      ...tseslint.configs.recommended,
      reactHooks.configs.flat.recommended,
      reactRefresh.configs.vite,
      eslintConfigPrettier,
    ],
    languageOptions: {
      ecmaVersion: 2023,
      globals: globals.browser,
      parserOptions: {
        projectService: true,
        tsconfigRootDir: import.meta.dirname,
      },
    },
    rules: {
      'react-refresh/only-export-components': ['warn', { allowConstantExport: true }],
    },
  },
  {
    // Upstream shadcn primitives intentionally export variants and helpers
    // alongside components; Fast Refresh warnings are noise here.
    files: ['src/components/ui/**/*.{ts,tsx}'],
    rules: {
      'react-refresh/only-export-components': 'off',
    },
  },
  {
    // Component-first policy: feature code composes web/src/components/ui primitives.
    // Raw interactive elements are allowed only in the primitive directory itself.
    files: ['src/**/*.{ts,tsx}'],
    ignores: ['src/components/ui/**', 'src/generated/**'],
    rules: {
      'no-restricted-syntax': [
        'error',
        {
          selector:
            'JSXOpeningElement[name.name=/^(button|input|select|option|optgroup|textarea|label|fieldset|legend|progress|meter|output|table|thead|tbody|tfoot|tr|th|td|caption|dialog|details|summary|hr|nav)$/]',
          message:
            'Use the installed shadcn primitive instead of raw HTML. See web/AGENTS.md and web/src/components/ui/README.md.',
        },
      ],
    },
  },
])
