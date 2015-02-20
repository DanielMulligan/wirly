var wirlyApp = angular.module('wirlyApp', []);

wirlyApp.controller('ProjectMembershipController', function ($scope, $window) {

    $scope.projectId = $window.Project.Id;
    //$scope.projectId = 122;
    $scope.UsersInProject = $window.UsersInProject;
    $scope.UsersNotInProject = $window.UsersNotInProject;

    $scope.AddUserToProject = function (user) {
        var index = $scope.UsersNotInProject.map(function (user) {
            return user.Id;
        }).indexOf(user.Id);

        $scope.UsersNotInProject.splice(index, 1);
        $scope.UsersInProject.push({ Id: user.Id, Name: user.Name, Email: user.Email });
    }

    $scope.RemoveUserFromProject = function (user) {
        var index = $scope.UsersInProject.map(function (user) {
            return user.Id;
        }).indexOf(user.Id);

        $scope.UsersInProject.splice(index, 1);
        $scope.UsersNotInProject.push({ Id: user.Id, Name: user.Name, Email: user.Email });
    }
});