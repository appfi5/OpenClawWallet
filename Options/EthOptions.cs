namespace OpenClawWalletServer.Options;

/// <summary>
/// Eth 配置
/// </summary>
public class EthOptions
{
    /// <summary>
    /// Eth Erc20 Transfer Abi
    /// </summary>
    public string EthERC20TransferAbi { get; set; } = """
                                                      [{
                                                          inputs: [
                                                              { internalType: 'address', name: 'to', type: 'address' },
                                                              { internalType: 'uint256', name: 'value', type: 'uint256' },
                                                          ],
                                                          name: 'transfer',
                                                          outputs: [{ internalType: 'bool', name: '', type: 'bool' }],
                                                          stateMutability: 'nonpayable',
                                                          type: 'function',
                                                      }]
                                                      """;

    /// <summary>
    /// Eth SafeWallet Abi
    /// </summary>
    public string EthSafeWalletAbi { get; set; } =
        """
        [{
            "inputs": [
                {
                    "internalType": "address",
                    "name": "to",
                    "type": "address"
                },
                {
                    "internalType": "uint256",
                    "name": "value",
                    "type": "uint256"
                },
                {
                    "internalType": "bytes",
                    "name": "data",
                    "type": "bytes"
                },
                {
                    "internalType": "enum Enum.Operation",
                    "name": "operation",
                    "type": "uint8"
                },
                {
                    "internalType": "uint256",
                    "name": "safeTxGas",
                    "type": "uint256"
                },
                {
                    "internalType": "uint256",
                    "name": "baseGas",
                    "type": "uint256"
                },
                {
                    "internalType": "uint256",
                    "name": "gasPrice",
                    "type": "uint256"
                },
                {
                    "internalType": "address",
                    "name": "gasToken",
                    "type": "address"
                },
                {
                    "internalType": "address payable",
                    "name": "refundReceiver",
                    "type": "address"
                },
                {
                    "internalType": "bytes",
                    "name": "signatures",
                    "type": "bytes"
                }
            ],
            "name": "execTransaction",
            "outputs": [
                {
                    "internalType": "bool",
                    "name": "success",
                    "type": "bool"
                }
            ],
            "stateMutability": "payable",
            "type": "function"
        },{
               "inputs": [
                   {
                       "internalType": "address",
                       "name": "to",
                       "type": "address"
                   },
                   {
                       "internalType": "uint256",
                       "name": "value",
                       "type": "uint256"
                   },
                   {
                       "internalType": "bytes",
                       "name": "data",
                       "type": "bytes"
                   },
                   {
                       "internalType": "enum Enum.Operation",
                       "name": "operation",
                       "type": "uint8"
                   },
                   {
                       "internalType": "uint256",
                       "name": "safeTxGas",
                       "type": "uint256"
                   },
                   {
                       "internalType": "uint256",
                       "name": "baseGas",
                       "type": "uint256"
                   },
                   {
                       "internalType": "uint256",
                       "name": "gasPrice",
                       "type": "uint256"
                   },
                   {
                       "internalType": "address",
                       "name": "gasToken",
                       "type": "address"
                   },
                   {
                       "internalType": "address",
                       "name": "refundReceiver",
                       "type": "address"
                   },
                   {
                       "internalType": "uint256",
                       "name": "_nonce",
                       "type": "uint256"
                   }
               ],
               "name": "getTransactionHash",
               "outputs": [
                   {
                       "internalType": "bytes32",
                       "name": "",
                       "type": "bytes32"
                   }
               ],
               "stateMutability": "view",
               "type": "function"
           },{
            "inputs": [
            ],
            "name": "nonce",
            "outputs": [
                {
                    "internalType": "uint256",
                    "name": "",
                    "type": "uint256"
                }
            ],
            "stateMutability": "view",
            "type": "function"
        }]
        """;
}