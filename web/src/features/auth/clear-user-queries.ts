import type { QueryClient } from '@tanstack/react-query'

import { authQueryKeys } from './query-keys'

export function clearUserScopedQueries(queryClient: QueryClient) {
  queryClient.removeQueries({ queryKey: authQueryKeys.all() })
}
