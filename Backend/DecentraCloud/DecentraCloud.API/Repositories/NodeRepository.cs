using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DecentraCloud.API.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly IMongoCollection<Node> _nodes;

        public NodeRepository(IMongoDatabase database)
        {
            _nodes = database.GetCollection<Node>("Nodes");
        }

        public async Task AddNode(Node node)
        {
            await _nodes.InsertOneAsync(node);
        }

        public async Task<Node> GetNodeById(string nodeId)
        {
            return await _nodes.Find(n => n.Id == nodeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateNode(Node node)
        {
            var result = await _nodes.ReplaceOneAsync(n => n.Id == node.Id, node);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
