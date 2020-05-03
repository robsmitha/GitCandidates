"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AuthGuard = /** @class */ (function () {
    function AuthGuard(auth, router) {
        this.auth = auth;
        this.router = router;
    }
    AuthGuard.prototype.canActivate = function (next, state) {
        if (this.auth.isLoggedIn) {
            return true;
        }
        else {
            this.router.navigateByUrl('sign-in');
            return false;
        }
    };
    return AuthGuard;
}());
exports.AuthGuard = AuthGuard;
//# sourceMappingURL=auth.guard.js.map
