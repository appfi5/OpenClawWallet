import { Button, Form, Input, message } from "antd";
import S from "../../index.module.less";
import request from "../../../../utils/request";
import { useState } from "react";
import useUserInfoStore from "../../../../store/userInfo.ts";

async function login(password: string) {
  return request<boolean>(`/api/authorization/login`, {
    method: 'POST',
    data: { password },
  });
}

const LoginWPassword = (props) => {
  const [loading, setLoading] = useState(false)
  const { login: loginSuccess } = useUserInfoStore()

  return <>
    <Form
      style={{width: 350}}
      onFinish={async values => {
        setLoading(true)
        const flag = await login(values.password).finally(() => setLoading(false))
        if (flag) {
          message.success('登录成功');
          loginSuccess()
        }}
    }
    >
      <Form.Item name="password">
        <Input.Password size="large" placeholder="输入密码" />
      </Form.Item>
      <Form.Item>
        <Button
          type="primary"
          block
          htmlType="submit"
          size="large"
          className={S.submit}
          loading={loading}
        >登录</Button>
      </Form.Item>
    </Form>
  </>
}

export default LoginWPassword;