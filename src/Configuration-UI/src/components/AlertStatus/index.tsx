import { Alert } from "antd";
import { useInterval, useRequest } from "ahooks";
import { agentStatus } from "../../service";
import { useEffect } from "react";
import to from "await-to-js";
import useUserInfoStore from "../../store/userInfo.ts";

const AlertStatus = () => {
  const { logout } = useUserInfoStore()
  const { data: statusMsg, runAsync: getStatus } = useRequest(agentStatus, {
    manual: true,
  })

  const clear = useInterval(() => {
    getStatus()
  }, 5000, {immediate: true});

  useEffect(() => {
    return () => {
      clear()
    }
  }, []);

  if (!statusMsg) return null

  return <Alert message={"服务器" + statusMsg} banner type="info" />
}

export default AlertStatus;