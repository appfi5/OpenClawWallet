import { create } from 'zustand'
import createSelectors from "./helper/createSelectors.ts";

export enum LOGIN_TYPE {
  UNKNOWN,
  LOGGED,
  UNLOGGED,
}

interface UserInfoStore {
  loggedType: LOGIN_TYPE;
  logout: () => void
  login: () => void
}

const useUserInfoStore = createSelectors(
  create<UserInfoStore>((set, get) => ({
    loggedType: LOGIN_TYPE.UNKNOWN,

    login: () => {
      set(() => ({ loggedType: LOGIN_TYPE.LOGGED }))
    },

    logout: () => {
      set(() => ({ loggedType: LOGIN_TYPE.UNLOGGED }))
    },
  })),
)

export default useUserInfoStore
