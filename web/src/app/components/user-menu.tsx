import { ChevronDownIcon, LogOutIcon, ShieldIcon } from 'lucide-react'

import { Avatar, AvatarFallback } from '@/components/ui/avatar'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Spinner } from '@/components/ui/spinner'
import { useLogout } from '@/features/auth/hooks'
import { useAuthStore } from '@/store/auth-store'

export function UserMenu() {
  const status = useAuthStore((state) => state.status)
  const displayName = useAuthStore((state) => state.displayName)
  const email = useAuthStore((state) => state.email)
  const role = useAuthStore((state) => state.role)
  const { logout, isPending } = useLogout()

  if (status !== 'authenticated') {
    return null
  }

  const menuLabel = displayName ?? 'Account'
  const initials =
    menuLabel
      .split(/\s+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((part) => part[0]?.toUpperCase() ?? '')
      .join('') || '?'

  return (
    <DropdownMenu>
      <DropdownMenuTrigger
        render={<Button variant="ghost" size="lg" className="gap-2" aria-label="Open user menu" />}
      >
        <Avatar size="sm">
          <AvatarFallback className="bg-primary/10 text-primary font-medium">
            {initials}
          </AvatarFallback>
        </Avatar>
        <span className="hidden text-sm font-medium sm:inline">{menuLabel}</span>
        <ChevronDownIcon className="text-muted-foreground size-4 shrink-0" aria-hidden />
      </DropdownMenuTrigger>

      <DropdownMenuContent align="end" className="w-56">
        <DropdownMenuGroup>
          <DropdownMenuLabel className="flex flex-col gap-1.5 p-2">
            <span className="text-foreground text-sm font-semibold">{menuLabel}</span>
            {email ? (
              <span className="text-muted-foreground truncate text-xs font-normal">{email}</span>
            ) : null}
            <div className="mt-1 flex items-center gap-1.5">
              <Badge variant="secondary" className="w-fit text-xs font-normal">
                <ShieldIcon data-icon="inline-start" className="size-3" />
                {role ?? 'User'}
              </Badge>
            </div>
          </DropdownMenuLabel>
        </DropdownMenuGroup>

        <DropdownMenuSeparator />

        <DropdownMenuGroup>
          <DropdownMenuItem
            variant="destructive"
            disabled={isPending}
            onClick={() => logout()}
            className="cursor-pointer px-2 py-1.5"
          >
            {isPending ? (
              <Spinner data-icon="inline-start" />
            ) : (
              <LogOutIcon data-icon="inline-start" />
            )}
            Log out
          </DropdownMenuItem>
        </DropdownMenuGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
