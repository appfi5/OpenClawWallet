import { Button, Form, Input, Modal } from "antd";
import S from "./index.module.less";
import { FC, useEffect, useState } from "react";
import { configAgentInfo, AgentInfoType } from "../../service";

type SettingModalProps = {
  agentInfo: AgentInfoType;
  reload: () => void
}

const SettingModal: FC<SettingModalProps> = (props) => {
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm();

  const onClose = () => {
    setOpen(false);
    form.resetFields();
  }

  useEffect(() => {
    if (open && props.agentInfo) {
      form.setFieldsValue(props.agentInfo)
    }
  }, [open]);

  return <>
    <Button className={S.button} onClick={() => setOpen(true)}>设置</Button>
    <Modal
      className={S.modal}
      open={open}
      title={'设置'}
      onCancel={onClose}
      onOk={() => form.submit()}
      destroyOnClose
    >
      <Form
        form={form}
        labelCol={{ span: 5 }}
        onFinish={async values => {
          await configAgentInfo(values)
          onClose();
          props.reload();
        }}
      >
        <Form.Item
          label="服务器地址"
          name="serverUrl"
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="access token"
          name="accessToken"
        >
          <Input.TextArea />
        </Form.Item>
      </Form>
    </Modal>
  </>
}

export default SettingModal;