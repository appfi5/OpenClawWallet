import { Button, message, Space } from "antd";
import { ProForm, ProFormDigit, ProFormText } from "@ant-design/pro-form";
import request from "@/utils/request";
import { useEffect, useState } from "react";
import { useBoolean, useRequest } from "ahooks";

export type SystemConfigData = {
  id: string;
  singleTransactionMaxLimit: string;
  createdAt: string;
  updatedAt: string;
}

async function createGlobalConfig(data: any) {
  return request<string>(`/api/global-config/create`, {
    method: 'POST',
    data,
  });
}

async function getGlobalConfig() {
  return request<SystemConfigData>(
    `/api/global-config`,
    { method: 'GET' },
  );
}

async function editGlobalConfig(data: any) {
  return request<boolean>(`/api/global-config/edit`, {
    method: 'POST',
    data,
  });
}

const AgentConfig = () => {
  const [form] = ProForm.useForm();
  const [readonly, setReadonly] = useState(true);
  const { data, run: reload } = useRequest(async () => {
    const res = await getGlobalConfig();
    if(res && ('singleTransactionMaxLimit' in res) && !res.id) {
      res.id = `${Math.random()}`.slice(2);
    }
    return res;
  });

  useEffect(() => {
    if (data) {
      form.setFieldsValue(data);
    }
  }, [data]);

  return <ProForm
    form={form}
    layout="horizontal"
    readonly={readonly}
    requiredMark={!readonly}
    submitter={{
      render({ submit }) {
        return (
          <Space>
            {readonly ? (
              <Button type="primary" onClick={() => setReadonly(false)}>
                编辑
              </Button>
            ) : (
              <>
                <Button
                  onClick={() => {
                    setReadonly(true);
                    form.resetFields();
                    form.setFieldsValue(data);
                  }}
                >
                  取消
                </Button>
                <Button type="primary" onClick={submit}>
                  提交
                </Button>
              </>
            )}
          </Space>
        );
      },
    }}
    onFinish={async (values) => {
      const flag = await (values.id
        ? editGlobalConfig(values)
        : createGlobalConfig(values));
      if (flag) {
        message.success('修改成功');
        reload();
        setReadonly(true);
      }
    }}
  >
    <ProFormText name="id" hidden />
    <ProFormDigit
      name="singleTransactionMaxLimit"
      label="单笔金额限额"
      width={180}
      min={0}
      max={5000}
      fieldProps={{ stringMode: true, precision: 6 }}
      rules={[{ required: true, message: "请输入单笔金额限额" }]}
    />
  </ProForm>
}

export default AgentConfig;