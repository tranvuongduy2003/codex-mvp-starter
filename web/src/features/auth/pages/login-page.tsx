import { Link, useLocation } from 'react-router-dom'

import { PageTitle } from '@/app/components/page-title'
import { paths } from '@/app/paths'
import { Alert, AlertDescription } from '@/components/ui/alert'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import type { LoginLocationState } from '../schemas'

import { LoginForm } from '../components'

export function LoginPage() {
  const location = useLocation()
  const locationState = location.state as LoginLocationState | null
  const sessionExpired = locationState?.reason === 'session-expired'

  return (
    <Card className="w-full shadow-md">
      <PageTitle title="Log in" />

      <CardHeader>
        <CardTitle className="text-2xl">
          <h1 className="text-2xl font-semibold tracking-tight">Log in</h1>
        </CardTitle>
        <CardDescription>Sign in to your account.</CardDescription>
      </CardHeader>

      <CardContent className="flex flex-col gap-6">
        {sessionExpired ? (
          <Alert>
            <AlertDescription>Your session has expired. Please log in again.</AlertDescription>
          </Alert>
        ) : null}

        <LoginForm />
      </CardContent>

      <CardFooter className="justify-center">
        <p className="text-muted-foreground text-center text-sm">
          No account?{' '}
          <Link to={paths.register} className="link-primary">
            Register
          </Link>
        </p>
      </CardFooter>
    </Card>
  )
}
