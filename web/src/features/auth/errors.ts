/**
 * Auth feature error mapping.
 * Translates API error responses into React Hook Form field/root errors.
 */
import type { FieldPath, UseFormSetError } from 'react-hook-form'

import { ApiError } from '@/types/api-problem'

import type { LoginFormValues, RegisterFormValues } from './schemas'

// Login --------------------------------------------------------------------

export const loginTransientErrorMessage = 'Something went wrong. Please try again.'

export const loginInvalidCredentialsMessage = 'Email or password is incorrect.'

export const loginCookiesRequiredMessage =
  'Cookies are required to stay signed in. Enable cookies for this site and try again.'

export function applyLoginApiError(
  error: unknown,
  setError: UseFormSetError<LoginFormValues>,
): void {
  if (!(error instanceof ApiError)) {
    setError('root', { message: loginTransientErrorMessage })
    return
  }

  if (error.problem.code === 'INTERNAL_ERROR' || error.status >= 500) {
    setError('root', { message: loginTransientErrorMessage })
    return
  }

  if (error.problem.code === 'INVALID_CREDENTIALS' || error.status === 401) {
    setError('root', { message: loginInvalidCredentialsMessage })
    return
  }

  const validationErrors = error.problem.errors
  if (!validationErrors) {
    return
  }

  for (const [field, messages] of Object.entries(validationErrors)) {
    setError(field as keyof LoginFormValues, { message: messages.join(' ') })
  }
}

// Register -----------------------------------------------------------------

export const registerTransientErrorMessage = 'Something went wrong. Please try again.'

type RegisterFormField = FieldPath<RegisterFormValues> | 'root'

const registerFieldErrorMessages: Record<
  string,
  Partial<Record<keyof RegisterFormValues, string>>
> = {
  EMAIL_TAKEN: {
    email: 'An account with this email already exists.',
  },
}

function setRegisterRootError(
  setError: UseFormSetError<RegisterFormValues>,
  message: string,
): void {
  setError('root' as RegisterFormField, { message })
}

export function applyRegisterApiError(
  error: unknown,
  setError: UseFormSetError<RegisterFormValues>,
): void {
  if (!(error instanceof ApiError)) {
    setRegisterRootError(setError, registerTransientErrorMessage)
    return
  }

  const code = error.problem.code
  if (code === 'INTERNAL_ERROR' || error.status >= 500) {
    setRegisterRootError(setError, registerTransientErrorMessage)
    return
  }

  if (code) {
    const mapped = registerFieldErrorMessages[code]
    if (mapped) {
      for (const [field, message] of Object.entries(mapped)) {
        setError(field as keyof RegisterFormValues, { message })
      }
      return
    }
  }

  const validationErrors = error.problem.errors
  if (!validationErrors) {
    return
  }

  for (const [field, messages] of Object.entries(validationErrors)) {
    setError(field as keyof RegisterFormValues, { message: messages.join(' ') })
  }
}
