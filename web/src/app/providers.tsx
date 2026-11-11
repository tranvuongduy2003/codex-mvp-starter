import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { useEffect } from 'react'
import { HelmetProvider } from 'react-helmet-async'
import { RouterProvider } from 'react-router-dom'

import { paths } from '@/app/paths'
import { router } from '@/app/router'
import { Toaster } from '@/components/ui/sonner'
import { TooltipProvider } from '@/components/ui/tooltip'
import { useAuthStore } from '@/store/auth-store'
import { ApiError } from '@/types/api-problem'

// staleTime / gcTime keep the UI snappy while still re-fetching when data is
// genuinely stale. Mutations never retry; client errors are not retried either,
// only transient 5xx/network failures.
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 60_000,
      gcTime: 5 * 60_000,
      refetchOnWindowFocus: false,
      retry: (failureCount, error) => {
        if (error instanceof ApiError && error.status < 500) return false
        return failureCount < 2
      },
    },
    mutations: {
      retry: false,
    },
  },
})

function UnauthorizedListener() {
  const clearSession = useAuthStore((state) => state.clearSession)

  useEffect(() => {
    const handleUnauthorized = () => {
      if (useAuthStore.getState().status !== 'authenticated') {
        return
      }

      router.navigate(paths.login, {
        replace: true,
        state: { reason: 'session-expired' },
      })
      clearSession()
      queryClient.clear()
    }

    window.addEventListener('api:unauthorized', handleUnauthorized)
    return () => window.removeEventListener('api:unauthorized', handleUnauthorized)
  }, [clearSession])

  return null
}

export function AppProviders() {
  return (
    <HelmetProvider>
      <QueryClientProvider client={queryClient}>
        <TooltipProvider>
          <UnauthorizedListener />
          <RouterProvider router={router} />
          <Toaster richColors closeButton />
        </TooltipProvider>
      </QueryClientProvider>
    </HelmetProvider>
  )
}
