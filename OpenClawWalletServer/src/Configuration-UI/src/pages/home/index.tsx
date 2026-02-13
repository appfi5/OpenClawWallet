import S from './index.module.less';
import { AgentInfoType, delKey, getAgentInfo, getKeyConfigList, ListItemType } from "../../service";
import { useRequest } from "ahooks";
import { Affix, Descriptions, Popconfirm, Table, Tag, Typography } from "antd";
import AlertStatus from "../../components/AlertStatus";
import SettingModal from "../../components/SettingModal";
import AddAddressModal from "../../components/AddAddressModal";
import SettingHeader from "./components/SettingHeader";

const signTypeMap = {
  1: 'ETH',
  2: 'CKB'
}

const Home = () => {
  const { data: agentInfo = {} as AgentInfoType, runAsync: getAgent } = useRequest<AgentInfoType>(getAgentInfo)
  const { data: listData = [], runAsync: refreshList } = useRequest<ListItemType[]>(getKeyConfigList)

  const columns = [{
    title: 'ID',
    dataIndex: 'keyConfigId',
  },{
    title: '地址',
    dataIndex: 'address',
    render: (value, record) => {
      return <>
        <Tag>{signTypeMap[record.signType]}</Tag>
        {value}
      </>
    }
  },{
    title: '添加时间',
    dataIndex: 'createdAt',
    render: (value) => {
      return new Date(value).toLocaleString()
    }
  },{
    title: '操作',
    dataIndex: 'opt',
    render: (_, record) => {
      return <Popconfirm
        title={'确认删除？'}
        onConfirm={async () => {
          await delKey(record.keyConfigId)
          refreshList();
        }}
      >
        <Typography.Link>删除</Typography.Link>
      </Popconfirm>
    }
  }]


  return <>
    {/*<Affix>*/}
      <AlertStatus />
      <SettingHeader />
    {/*</Affix>*/}
    <div className={S.page}>
      <div className={S.info}>
        <Descriptions column={1}>
          <Descriptions.Item label="AgentID">{agentInfo?.code}</Descriptions.Item>
          <Descriptions.Item label="服务器地址">{agentInfo?.serverUrl}</Descriptions.Item>
          <Descriptions.Item label="access token">{agentInfo?.accessToken}</Descriptions.Item>
        </Descriptions>

        <SettingModal agentInfo={agentInfo} reload={getAgent} />

      </div>

      <div className={S.table}>
        <div className={S.tHeader}>
          <h4>地址列表</h4>
          <AddAddressModal reload={refreshList} />
        </div>

        <Table columns={columns} dataSource={listData} pagination={false} />
      </div>
    </div>
  </>
}

export default Home;