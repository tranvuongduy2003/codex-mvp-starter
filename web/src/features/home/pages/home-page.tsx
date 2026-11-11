import { KeyRoundIcon, ShieldCheckIcon, UserIcon } from 'lucide-react'

import { PageTitle } from '@/app/components/page-title'
import { Badge } from '@/components/ui/badge'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { useAuthStore } from '@/store/auth-store'

export function HomePage() {
  const userId = useAuthStore((state) => state.userId)
  const displayName = useAuthStore((state) => state.displayName)
  const email = useAuthStore((state) => state.email)
  const role = useAuthStore((state) => state.role)

  return (
    <div className="flex flex-col gap-8">
      <PageTitle title="Dashboard" />

      <section className="flex flex-col gap-2">
        <Badge variant="secondary" className="w-fit">
          Application shell
        </Badge>
        <h1 className="text-foreground text-3xl font-bold tracking-tight">
          Welcome back{displayName ? `, ${displayName}` : ''}
        </h1>
        <p className="text-muted-foreground max-w-2xl text-base">
          This starter keeps the account, session, and role foundations ready for your next product.
        </p>
      </section>

      <section className="grid gap-4 sm:grid-cols-3">
        <Card className="shadow-sm">
          <CardHeader className="pb-2">
            <CardDescription>Account</CardDescription>
            <CardTitle className="flex items-center gap-2 text-lg">
              <UserIcon className="text-primary size-4" aria-hidden />
              Active
            </CardTitle>
          </CardHeader>
          <CardContent className="text-muted-foreground text-sm">
            {email ?? 'No email loaded'}
          </CardContent>
        </Card>

        <Card className="shadow-sm">
          <CardHeader className="pb-2">
            <CardDescription>Role</CardDescription>
            <CardTitle className="flex items-center gap-2 text-lg">
              <ShieldCheckIcon className="text-primary size-4" aria-hidden />
              {role ?? 'User'}
            </CardTitle>
          </CardHeader>
          <CardContent className="text-muted-foreground text-sm">
            Server-side policies enforce protected operations.
          </CardContent>
        </Card>

        <Card className="shadow-sm">
          <CardHeader className="pb-2">
            <CardDescription>User ID</CardDescription>
            <CardTitle className="flex min-w-0 items-center gap-2 font-mono text-sm font-medium">
              <KeyRoundIcon className="text-primary size-4 shrink-0" aria-hidden />
              <span className="truncate">{userId ?? '-'}</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="text-muted-foreground text-sm">
            Stable account identifier from the backend session.
          </CardContent>
        </Card>
      </section>
    </div>
  )
}
