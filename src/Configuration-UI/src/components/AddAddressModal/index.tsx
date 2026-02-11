import { Button, Form, Input, Modal, Select } from "antd";
import S from "./index.module.less";
import { useState } from "react";
import { addKeyConfig } from "../../service";

const AddAddressModal = (props: {reload: () => void}) => {
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm();

  const onClose = () => {
    setOpen(false);
    form.resetFields();
  }

  return <>
    <Button type={'primary'} onClick={() => setOpen(true)}>新增地址</Button>
    <Modal
      className={S.modal}
      open={open}
      title={'新增地址'}
      onCancel={onClose}
      onOk={() => form.submit()}
      destroyOnClose
    >
      <Form
        form={form}
        labelCol={{ span: 5 }}
        onFinish={async values => {
          await addKeyConfig({...values, signType: Number(values.signType)});
          onClose()
          props.reload();
        }}
      >
        <Form.Item
          label="地址类型"
          name="signType"
          rules={[{ required: true, message: '请选择地址类型' }]}
        >
          <Select>
            <Select.Option value="1">ETH</Select.Option>
            <Select.Option value="2">CKB</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item
          label="地址"
          name="address"
          rules={[{ required: true, message: '请输入地址' }]}
        >
          <Input.TextArea />
        </Form.Item>
        <Form.Item
          label="私钥"
          name="privateKey"
          rules={[{ required: true, message: '请输入私钥' }]}
        >
          <Input.TextArea />
        </Form.Item>
      </Form>
    </Modal>
  </>
}

export default AddAddressModal;