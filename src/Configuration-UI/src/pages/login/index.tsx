import S from './index.module.less';
import { Button, Form, Input, ConfigProvider } from "antd";
import InitPassword from "./components/InitPassword";
import { useEffect, useState } from "react";
import LoginWPassword from "./components/LoginWPassword";
import request from "../../utils/request";
import useUserInfoStore from "../../store/userInfo.ts";

enum LOGIN_STEP {
  INIT,
  LOGIN,
}

async function hadInit() {
  return request<boolean>(`/api/authorization/had-init`, {
    method: 'GET',
    throwIfBizDataIsFalse: false,
  });
}

const Login = () => {
  const [step, setStep] = useState<LOGIN_STEP | undefined>()

  const checkInit = async () => {
    const flag = await hadInit()
    setStep(flag ? LOGIN_STEP.LOGIN : LOGIN_STEP.INIT)
  }

  useEffect(() => {
    checkInit()
  }, []);

  return (
    <ConfigProvider
      theme={{
        token: {
          motion: false
        },
        components: {
          Input: {
            controlHeightLG: 54,
          },
          Form: {
            labelFontSize: 16,
            labelHeight: '24px',
          },
          Button: {
            controlHeightLG: 54,
            contentFontSizeLG: 18,
          },
        }
      }}
    >
      <div className={S.page}>
        <div className={S.container}>
          <h1 style={{marginBottom: 40}}>Welcome ~</h1>
          {step === LOGIN_STEP.LOGIN && <LoginWPassword />}
          {step === LOGIN_STEP.INIT && <InitPassword goLogin={() => setStep(LOGIN_STEP.LOGIN)} />}
        </div>
      </div>
    </ConfigProvider>
  );
}

export default Login;