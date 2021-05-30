namespace Tasks.Api.Exceptions
{
    public static class AppExceptions
    {
        public const string RoomNotFoundException = "Room with given id was not found!";

        public const string TaskNotFoundException = "Task with given id was not found!";

        public const string TaskInRoomNotFoundException = "Task with given id was not found in room!";

        public const string RoleNotFoundException = "Role with given id was not found!";

        private const string InsufficientRightsException = "Insufficient rights! ";

        
        public const string NotRoomMemberException =
            InsufficientRightsException + "The user is not a member of the room!";

        public const string NoAccessToTaskException =
            InsufficientRightsException + "The user does not have access to this task!";

        public const string CreatorOnlyCanPerformThisActionException =
            InsufficientRightsException + "Only owner can perform this action!";

        public const string CreatorOrAdministratorOnlyCanDoThisException =
            InsufficientRightsException + "Only owner or administrator can perform this action!";

        public const string IncorrectUrlException = "Incorrect link!";

        public const string AlreadyMember = "You are already a member of the room!";
        
        public const string CreatorNotFound = "Creator of the task was not found!";
    }
}