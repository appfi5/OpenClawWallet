import request from "../utils/request";

export type AgentInfoType = {
  code: string;
  serverUrl: string;
  accessToken: string;
}

export function getAgentInfo() {
  return request<AgentInfoType>(`/api/agent-config/agent-info`, {
    method: 'GET',
  });
}

export function configAgentInfo(params: Omit<AgentInfoType, 'code'>) {
  return request<boolean>(`/api/agent-config/config-info`, {
    method: 'POST',
    data: params,
  });
}

export type ListItemType = {
  keyConfigId: string;
  address: string;
  createdAt: string;
  signType: number
}

export function getKeyConfigList() {
  return request<ListItemType[]>(`/api/agent-config/key-config-list`, {
    method: 'GET',
  });
}

export type AddKeyParamsType = {
	signType: number;
	address: string;
	privateKey: string;
}

export function addKeyConfig(params: AddKeyParamsType) {
  return request<boolean>(`/api/agent-config/add-key`, {
    method: 'POST',
    data: params,
  });
}

export function delKey(keyConfigId: string) {
  return request<boolean>(`/api/agent-config/delete-key`, {
    method: 'POST',
    data: { keyConfigId },
  });
}

const statusMsgMap = {
  'Unknown': '连接未知',
  'Uninitialized': '连接未初始化',
  'Disconnected': '未连接',
  'Connecting': '连接中',
  'Connected': '连接成功',
}

export async function agentStatus() {
  const result = await request<{status: string}>(`/api/agent-config/agent-status`, {
    method: 'GET',
  });

  return statusMsgMap[result?.status]
}

export async function validateLogin() {
  return request<{status: string}>(`/api/agent-config/agent-status`, {
    method: 'GET',
    skipErrorHandler: true
  });
}