using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad
{
    public static class CodeCommand
    {
        public const string OK = "OK";
        public const string ERROR = "ERROR";
        public const string NONE = "NONE";
        public const string EXIT = "EXIT";

        public const string AUTHORIZATION = "AUTHORIZATION";
        public const string AUTHORIZATION_FAIL = "AUTHORIZATION_FAIL";

        public const string GET_ROLE = "GET_ROLE";

        public const string REGISTRATION = "REGISTRATION";
        public const string REGISTRATION_USER_EXISTS = "REGISTRATION_USER_EXISTS";

        public const string GET_ORGANIZATION = "GET_ORGANIZATION";

        public const string GET_JOB_TYPE = "GET_JOB_TYPE";
        public const string GET_ALL_JOB_TYPES = "GET_ALL_JOB_TYPES";

        public const string GET_USER_TYPE = "GET_USER_TYPE";
        public const string GET_ALL_USER_TYPES = "GET_ALL_USER_TYPES";

        public const string GET_REGION = "GET_REGION";
        public const string GET_ALL_REGIONS = "GET_ALL_REGIONS";

        public const string GET_ALL_CURRENCIES = "GET_ALL_CURRENCIES";
        public const string GET_CURRENCY = "GET_CURRENCY";

        public const string SAVE_USER = "SAVE_USER";
        public const string SAVE_ORGANIZATION = "SAVE_ORGANIZATION";
        public const string DELETE_OLD_ORGANIZATION = "DELETE_OLD_ORGANIZATION";

        public const string SAVE_ORDER = "SAVE_ORDER";
        public const string GET_ALL_ORDERS = "GET_ALL_ORDERS";
        public const string DELETE_ORDER = "DELETE_ORDER";
        public const string UPDATE_ORDER = "UPDATE_ORDER";
        public const string GET_USER_BY_ORDER = "GET_USER_BY_ORDER";
        public const string GET_ORDER_CUSTOMER = "GET_ORDER_CUSTOMER";

        public const string RESPOND_ORDER = "RESPOND_ORDER";
        public const string RESPOND_ORDER_EXISTS = "RESPOND_ORDER_EXISTS";

        public const string GET_PERFORMERS_BY_ORDER = "GET_PERFORMERS_BY_ORDER";

        public const string CHANGE_PASSWORD = "CHANGE_PASSWORD";
        public const string CHANGE_PASSWORD_OLD_FAIL = "CHANGE_PASSWORD_OLD_FAIL";

        public const string GET_ALL_INFO = "GET_ALL_INFO";

        public const string ADMIN_GET_ALL_USERS = "ADMIN_GET_ALL_USERS";
        public const string ADMIN_DELETE_INFO = "ADMIN_DELETE_INFO";
        public const string ADMIN_SAVE_INFO = "ADMIN_SAVE_INFO";
        public const string ADMIN_DELETE_USER = "ADMIN_DELETE_USER";


    }
}
