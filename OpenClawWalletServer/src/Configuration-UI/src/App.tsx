import './app.css'
import Home from "./pages/home";
import Login from "./pages/login";
import { useEffect } from "react";
import to from "await-to-js";
import { validateLogin } from "./service";
import useUserInfoStore, { LOGIN_TYPE } from "./store/userInfo.ts";

function App() {
  const { loggedType, logout, login } = useUserInfoStore()

  const validate = async () => {
    const [err, data] = await to(validateLogin())
    if (err && err.status === 401) {
      logout();
    }
    if (!err) {
      login()
    }
  }

  useEffect(() => {
    validate();
  }, []);

  if (loggedType === LOGIN_TYPE.LOGGED) {
    return <Home />
  }

  return <Login />
}

export default App
