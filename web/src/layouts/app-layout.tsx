import { ShieldCheckIcon } from 'lucide-react'
import { Link, NavLink, Outlet, useLocation } from 'react-router-dom'

import { UserMenu } from '@/app/components/user-menu'
import { paths } from '@/app/paths'
import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
} from '@/components/ui/navigation-menu'
import { cn } from '@/lib/utils'

const navItems = [{ path: paths.home, label: 'Dashboard', end: true }] as const

function MainNavigation() {
  const { pathname } = useLocation()

  return (
    <NavigationMenu aria-label="Main navigation" className="hidden md:flex">
      <NavigationMenuList className="justify-start gap-1">
        {navItems.map((item) => {
          const isActive = item.end ? pathname === item.path : pathname.startsWith(item.path)

          return (
            <NavigationMenuItem key={item.path}>
              <NavigationMenuLink
                render={<NavLink to={item.path} end={item.end} />}
                className={cn(
                  'text-muted-foreground px-3 py-2 font-medium',
                  isActive && 'bg-primary/10 text-primary hover:bg-primary/10',
                )}
              >
                {item.label}
              </NavigationMenuLink>
            </NavigationMenuItem>
          )
        })}
      </NavigationMenuList>
    </NavigationMenu>
  )
}

export function AppLayout() {
  return (
    <div className="flex min-h-screen flex-col">
      <header
        className="border-border bg-card sticky top-0 z-40 border-b shadow-sm"
        style={{ viewTransitionName: 'site-header' }}
      >
        <div className="store-container flex h-16 items-center justify-between gap-6">
          <div className="flex min-w-0 items-center gap-8">
            <Link
              to={paths.home}
              className="text-primary flex shrink-0 items-center gap-2 font-semibold tracking-tight"
            >
              <span className="bg-primary text-primary-foreground flex size-8 items-center justify-center rounded-lg">
                <ShieldCheckIcon className="size-4" aria-hidden />
              </span>
              <span className="text-foreground hidden text-base sm:inline">Solution</span>
            </Link>

            <MainNavigation />
          </div>

          <UserMenu />
        </div>
      </header>

      <main className="store-container flex-1 py-8 lg:py-10">
        <Outlet />
      </main>

      <footer className="border-border bg-card mt-auto border-t">
        <div className="store-container text-muted-foreground flex h-14 items-center justify-between text-xs">
          <p>Solution boilerplate</p>
          <p>Users, authentication, and authorization</p>
        </div>
      </footer>
    </div>
  )
}
