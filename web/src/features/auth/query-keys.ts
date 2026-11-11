/**
 * Centralized query key factory for the auth feature.
 * Import from here instead of using inline string arrays.
 */
export const authQueryKeys = {
  all: () => ['auth'] as const,
  session: () => ['auth', 'session'] as const,
} as const
