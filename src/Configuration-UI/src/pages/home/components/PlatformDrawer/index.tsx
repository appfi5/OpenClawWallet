import { Drawer, Typography } from "antd";
import { useBoolean } from "ahooks";
import AgentConfig from "./components/AgentConfig";
import WhiteList from "./components/WhiteList";

const { Link } = Typography;

const PlatFormDrawer = () => {
  const [open, { setTrue, setFalse }] = useBoolean(false);

  return <>
    <Link onClick={setTrue}>平台配置</Link>
    <Drawer title="平台配置" open={open} width={800} onClose={setFalse}>
      <AgentConfig />
      <WhiteList />
    </Drawer>
  </>
}

export default PlatFormDrawer;