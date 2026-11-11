import { Helmet } from 'react-helmet-async'

const APP_NAME = 'Solution'

interface PageTitleProps {
  /** Page-specific title. Rendered as "<title> | Solution" in the browser tab. */
  title: string
}

/**
 * Sets the browser tab title for a page.
 * Place inside any page component that is wrapped by HelmetProvider.
 */
export function PageTitle({ title }: PageTitleProps) {
  return (
    <Helmet>
      <title>
        {title} | {APP_NAME}
      </title>
    </Helmet>
  )
}
