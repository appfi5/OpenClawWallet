import S from './index.module.less';
import { Space } from 'antd';
import PlatFormDrawer from "../PlatformDrawer";
import ChangePassword from "../ChangePassword";

const SettingHeader = () => {
  return <div className={S.container}>
    <Space>
      <PlatFormDrawer />
      <ChangePassword />
    </Space>
  </div>
}

export default SettingHeader;