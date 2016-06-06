//using System.Linq;
//using MongoRepository;
//using AT.Core.Cqrs;
//using AT.SampleApp.Cqrs.Aggregate;
//using AT.SampleApp.Cqrs.Query;
//using AT.SampleApp.Cqrs.QueryResult;

//namespace AT.SampleApp.Cqrs.QueryHandler
//{
//    public class TasksByStatusQueryHandler : IQueryHandler<TasksByStatusQuery, TasksByStatusQueryResult>
//    {
//        private readonly IRepository<Employee> _taskRepository;

//        public TasksByStatusQueryHandler(IRepository<Employee> taskRepository)
//        {
//            _taskRepository = taskRepository;
//        }

//        public TasksByStatusQueryResult Retrieve(TasksByStatusQuery query)
//        {
//            TasksByStatusQueryResult result = new TasksByStatusQueryResult();
//            result.Tasks = _taskRepository.All().Where(x => x.IsCompleted == query.IsCompleted).ToList();
//            result.LastUpdateForAnyTask = _taskRepository.All().Max(x => x.LastUpdated);
//            return result;
//        }
//    }
//}