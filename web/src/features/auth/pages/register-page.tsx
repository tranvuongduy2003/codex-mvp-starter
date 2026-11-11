import { Link } from 'react-router-dom'

import { PageTitle } from '@/app/components/page-title'
import { paths } from '@/app/paths'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

import { RegisterForm } from '../components'

export function RegisterPage() {
  return (
    <Card className="w-full shadow-md">
      <PageTitle title="Create account" />

      <CardHeader>
        <CardTitle className="text-2xl">
          <h1 className="text-2xl font-semibold tracking-tight">Create account</h1>
        </CardTitle>
        <CardDescription>Create an account to get started.</CardDescription>
      </CardHeader>

      <CardContent className="flex flex-col gap-6">
        <RegisterForm />
      </CardContent>

      <CardFooter className="justify-center">
        <p className="text-muted-foreground text-center text-sm">
          Already have an account?{' '}
          <Link to={paths.login} className="link-primary">
            Log in
          </Link>
        </p>
      </CardFooter>
    </Card>
  )
}
