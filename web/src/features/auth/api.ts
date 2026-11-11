import { apiClient, type ApiJsonBody, type ApiJsonResponse } from '@/lib/api'

export type LoginRequest = ApiJsonBody<'/api/auth/login', 'post'>

export type LoginUserResponse = ApiJsonResponse<'/api/auth/login', 'post'>

export type RegisterRequest = ApiJsonBody<'/api/users', 'post'>

export type UserRegistrationResponse = ApiJsonResponse<'/api/users', 'post', 201>

export function getCurrentUser(signal?: AbortSignal) {
  return apiClient.get<LoginUserResponse>('/api/auth/me', { signal })
}

export function login(request: LoginRequest, signal?: AbortSignal) {
  return apiClient.post<LoginUserResponse>('/api/auth/login', request, {
    signal,
    suppressErrorToast: true,
  })
}

export function register(request: RegisterRequest, signal?: AbortSignal) {
  return apiClient.post<UserRegistrationResponse>('/api/users', request, {
    signal,
    suppressErrorToast: true,
  })
}

export function logout(signal?: AbortSignal) {
  return apiClient.post<void>('/api/auth/logout', undefined, {
    signal,
    suppressErrorToast: true,
  })
}
