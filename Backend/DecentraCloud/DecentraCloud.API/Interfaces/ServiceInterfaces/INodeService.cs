using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Xml.Linq;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface INodeService
    {
        Task<Node> RegisterNode(NodeRegistrationDto nodeRegistrationDto);
        Task<bool> UpdateNodeStatus(NodeStatusDto nodeStatusDto);
    }

}
