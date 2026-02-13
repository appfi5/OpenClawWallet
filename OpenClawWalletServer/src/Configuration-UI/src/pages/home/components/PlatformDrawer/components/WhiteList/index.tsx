import { Button, Popconfirm, Space, Table, Typography } from "antd";
import S from "./index.module.less";
import AddWAddressModal from "../AddWAddressModal";
import request from "@/utils/request";
import { useRequest } from "ahooks";

async function getList() {
  return request<boolean>(
    `/api/whitelist-item/list`,
    { method: 'POST' },
  );
}

async function delAddress(whitelistItemId: string) {
  return request<boolean>(
    `/api/whitelist-item/delete`,
    { method: 'POST', data: { whitelistItemId } },
  );
}

const WhiteList = () => {
  const { data: listData = [], runAsync: refreshList } = useRequest<any[]>(getList)

  const columns = [{
    title: '白名单地址',
    dataIndex: 'address',
  }, {
    title: '单笔限额',
    dataIndex: 'singleTransactionMaxLimit',
  },{
    title: '操作',
    dataIndex: 'opt',
    render: (_, record) => {
      return <Space>
        <AddWAddressModal
          editInfo={record}
          trigger={<Typography.Link >编辑</Typography.Link>}
          reload={refreshList}
        />
        <Popconfirm
          title={'确认删除？'}
          onConfirm={async () => {
            await delAddress(record.whitelistItemId)
            refreshList();
          }}
        >
          <Typography.Link>删除</Typography.Link>
        </Popconfirm>
      </Space>
    }
  }];

  return <div className={S.table}>
    <div className={S.tHeader}>
      <h4>白名单列表</h4>
      <AddWAddressModal trigger={<Button type={'primary'}>新增白名单</Button>} reload={refreshList} />
    </div>

    <Table columns={columns} dataSource={listData} pagination={false} />
  </div>
}

export default WhiteList;