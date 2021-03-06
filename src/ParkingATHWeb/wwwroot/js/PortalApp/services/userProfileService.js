﻿(function () {
    'use strict';

    function userProfileService() {
        this.userData = {
            name: "",
            lastName: "",
            profilePhotoPath: "/images/user-avatars/avatar-placeholder.jpg",
            charges: 0,
            unreadMessages: 0
        }

        this.setInitials = function(name, lastName) {
            this.userData.name = name;
            this.userData.lastName = lastName;
        }

        this.getInitials = function() {
            return this.userData.name + " " + this.userData.lastName;
        }

        this.setProfilePhotoPath = function (imgId) {
            if (imgId != null) {
                this.userData.profilePhotoPath = "/images/user-avatars/" + imgId + ".jpg";
            }
            return this.userData.profilePhotoPath;
        }

        this.setCharges = function(charges) {
            this.userData.charges = charges;
        }

        this.decrementMessagesCount = function() {
            --this.userData.unreadMessages;
        }

        this.getUnreadMessages = function () {
            return this.userData.unreadMessages;
        }
    }

    angular.module('portalApp').service('userProfileService', userProfileService);
})();