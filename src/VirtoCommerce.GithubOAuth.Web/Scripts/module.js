// Call this to register your module to main application
var moduleName = 'GithubOAuth';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.GithubOAuthState', {
                    url: '/GithubOAuth',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'GithubOAuth.helloWorldController',
                                template: 'Modules/$(VirtoCommerce.GithubOAuth)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true,
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', '$state',
        function (mainMenuService, $state) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/GithubOAuth',
                icon: 'fa fa-cube',
                title: 'GithubOAuth',
                priority: 100,
                action: function () { $state.go('workspace.GithubOAuthState'); },
                permission: 'GithubOAuth:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
