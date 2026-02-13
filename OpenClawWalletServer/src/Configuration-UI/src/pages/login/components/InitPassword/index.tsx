import { Button, Form, Input, message } from "antd";
import S from "../../index.module.less";
import request from "../../../../utils/request";
import { useState } from "react";

type Props = {
  goLogin: () => void
}

async function initPassword(password: string) {
  return request<boolean>(`/api/authorization/init`, {
    method: 'POST',
    data: { password },
  });
}

const InitPassword = (props: Props) => {
  const { goLogin } = props;
  const [loading, setLoading] = useState(false)

  return <>
    <p className={S.tips}>初次见面，请先设置密码</p>
    <Form
      style={{width: 350}}
      onFinish={async values => {
        setLoading(true)
        const flag = await initPassword(values.password).finally(() => setLoading(false))
        if (flag) {
          message.success('设置成功')
          goLogin()
        }
      }}>
      <Form.Item name="password">
        <Input.Password size="large" placeholder="输入密码" />
      </Form.Item>
      <Form.Item>
        <Button
          type="primary"
          loading={loading}
          block
          htmlType="submit"
          size="large"
          className={S.submit}
        >设置密码</Button>
      </Form.Item>
    </Form>
  </>
}

export default InitPassword;