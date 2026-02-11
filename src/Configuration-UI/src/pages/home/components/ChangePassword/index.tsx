import { ModalForm, ProFormText } from "@ant-design/pro-form";
import { useBoolean } from "ahooks";
import { Typography } from "antd";
import request from "../../../../utils/request";
import useUserInfoStore from "../../../../store/userInfo.ts";
const { Link } = Typography;

async function modifyPassword(data: any) {
  return request<boolean>(`/api/authorization/modify-password`, {
    method: 'POST',
    data,
  });
}

const ChangePassword = () => {
  const { logout } = useUserInfoStore()
  const [open, { setTrue, setFalse }] = useBoolean(false);

  return <>
    <Link onClick={setTrue}>修改密码</Link>
    <ModalForm
      open={open}
      title={'修改密码'}
      width={500}
      modalProps={{
        destroyOnClose: true,
        onCancel: setFalse,
      }}
      onFinish={async (values) => {
        console.log(values);
        await modifyPassword(values);
        setFalse();
        logout()
      }}
    >
      <ProFormText.Password
        name="oldPassword"
        label="旧密码"
        placeholder="请输入旧密码"
        rules={[{ required: true, message: '请输入旧密码' }]}
      />
      <ProFormText.Password
        name="newPassword"
        label="新密码"
        placeholder="请输入新密码"
        rules={[{ required: true, message: '请输入新密码' }]}
      />
    </ModalForm>
  </>
}

export default ChangePassword;