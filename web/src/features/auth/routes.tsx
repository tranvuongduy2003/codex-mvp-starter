import type { RouteObject } from 'react-router-dom'

export const authRoutes: RouteObject[] = [
  {
    path: 'login',
    lazy: async () => {
      const { LoginPage } = await import('./pages/login-page')
      return { Component: LoginPage }
    },
  },
  {
    path: 'register',
    lazy: async () => {
      const { RegisterPage } = await import('./pages/register-page')
      return { Component: RegisterPage }
    },
  },
]
