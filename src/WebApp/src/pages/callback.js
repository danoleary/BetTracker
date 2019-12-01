import React from 'react'
import { ClipLoader } from 'react-spinners'
 
import Auth from '../services/auth'
import Layout from '../components/layout'
 
function Auth0CallbackPage() {
  const [mounted, setMounted] = useState(false)
    useEffect(
      () => {
        setMounted(true)
   
        const auth = new Auth()
        auth.handleAuthentication()
      },
      [mounted]
    )
 
  return (
    <Layout>
      <h1>
        This is the auth callback page, you should be redirected immediately.
      </h1>
      <ClipLoader sizeUnit="px" size={150} />
    </Layout>
  )
}
 
export default Auth0CallbackPage