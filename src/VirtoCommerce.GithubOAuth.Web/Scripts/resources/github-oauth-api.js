angular.module('GithubOAuth')
    .factory('GithubOAuth.webApi', ['$resource', function ($resource) {
        return $resource('api/github-oauth');
    }]);
