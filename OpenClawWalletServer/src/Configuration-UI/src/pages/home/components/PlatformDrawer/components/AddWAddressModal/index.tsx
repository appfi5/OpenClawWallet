import { Button, Form, Input, message, Modal, Select } from "antd";
import S from "./index.module.less";
import { cloneElement, useEffect, useState } from "react";
import { ProFormDigit, ProFormText } from "@ant-design/pro-form";
import request from "@/utils/request";

type ModifyAddressParamsType = {
  whitelistItemId: string;
  address: string;
  singleTransactionMaxLimit: number;
}

async function createAddress(data: any) {
  return request<boolean>(
    `/api/whitelist-item/create`,
    { method: 'POST', data },
  );
}

async function editAddress(data: ModifyAddressParamsType) {
  return request<boolean>(
    `/api/whitelist-item/modify`,
    { method: 'POST', data },
  );
}

const AddAddressModal = (props: {
  reload: () => void;
  trigger: React.ReactNode;
  editInfo?: ModifyAddressParamsType;
}) => {
  const { editInfo } = props;
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm();

  useEffect(() => {
    if (editInfo) {
      form.setFieldsValue(editInfo)
    }
  }, [editInfo]);

  const onClose = () => {
    setOpen(false);
    form.resetFields();
  }

  const type = editInfo ? '编辑' : '新增';

  return <>
    {cloneElement(props.trigger, { onClick: () => setOpen(true) })}
    <Modal
      className={S.modal}
      open={open}
      title={type + '白名单'}
      onCancel={onClose}
      onOk={() => form.submit()}
      destroyOnClose
    >
      <Form
        form={form}
        labelCol={{ span: 6 }}
        initialValues={editInfo}
        onFinish={async values => {
          const obj = {...values, address: values.address.trim()};
          const flag = await (values.whitelistItemId
            ? editAddress(obj)
            : createAddress(obj));
          message.success(type + '成功');
          onClose()
          props.reload();
        }}
      >
        <ProFormText name="whitelistItemId" hidden />
        <Form.Item
          label="地址"
          name="address"
          rules={[{ required: true, message: '请输入地址' }]}
        >
          <Input.TextArea />
        </Form.Item>
        <ProFormDigit
          name="singleTransactionMaxLimit"
          label="单笔金额限额"
          min={0}
          fieldProps={{ stringMode: true, precision: 6 }}
          rules={[{ required: true, message: "请输入单笔金额限额" }]}
        />
      </Form>
    </Modal>
  </>
}

export default AddAddressModal;