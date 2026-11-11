import { ShieldCheckIcon } from 'lucide-react'
import { Outlet } from 'react-router-dom'

export function AuthLayout() {
  return (
    <div className="bg-muted/40 flex min-h-screen flex-col">
      <header className="border-border bg-card border-b shadow-sm">
        <div className="mx-auto flex h-16 max-w-lg items-center justify-center gap-2 px-4">
          <span className="bg-primary text-primary-foreground flex size-8 items-center justify-center rounded-lg">
            <ShieldCheckIcon className="size-4" aria-hidden />
          </span>
          <span className="text-foreground text-lg font-semibold tracking-tight">Solution</span>
        </div>
      </header>

      <main className="flex flex-1 items-center justify-center px-4 py-12">
        <div className="w-full max-w-md">
          <Outlet />
        </div>
      </main>
    </div>
  )
}
