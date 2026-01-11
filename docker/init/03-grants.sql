--iam
GRANT USAGE ON SCHEMA iam TO iam_user;
GRANT CREATE ON SCHEMA iam TO iam_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA iam
GRANT ALL ON TABLES TO iam_user;

--catalog_pricing
GRANT USAGE ON SCHEMA catalog_pricing TO catalog_pricing_user;
GRANT CREATE ON SCHEMA catalog_pricing TO catalog_pricing_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA catalog_pricing
GRANT ALL ON TABLES TO catalog_pricing_user;

--billing_payment
GRANT USAGE ON SCHEMA billing_payment TO billing_payment_user;
GRANT CREATE ON SCHEMA billing_payment TO billing_payment_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA billing_payment
GRANT ALL ON TABLES TO billing_payment_user;

--crm
GRANT USAGE ON SCHEMA crm TO crm_user;
GRANT CREATE ON SCHEMA crm TO crm_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA crm
GRANT ALL ON TABLES TO crm_user;

--customer_operation
GRANT USAGE ON SCHEMA customer_operation TO customer_operation_user;
GRANT CREATE ON SCHEMA customer_operation TO customer_operation_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA customer_operation
GRANT ALL ON TABLES TO customer_operation_user;

--deadline_management
GRANT USAGE ON SCHEMA deadline_management TO deadline_management_user;
GRANT CREATE ON SCHEMA deadline_management TO deadline_management_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA deadline_management
GRANT ALL ON TABLES TO deadline_management_user;

--employee_incetive
GRANT USAGE ON SCHEMA employee_incetive TO employee_incetive_user;
GRANT CREATE ON SCHEMA employee_incetive TO employee_incetive_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA employee_incetive
GRANT ALL ON TABLES TO employee_incetive_user;

--infrastructure_management
GRANT USAGE ON SCHEMA infrastructure_management TO infrastructure_management_user;
GRANT CREATE ON SCHEMA infrastructure_management TO infrastructure_management_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA infrastructure_management
GRANT ALL ON TABLES TO infrastructure_management_user;

--notification
GRANT USAGE ON SCHEMA notification TO notification_user;
GRANT CREATE ON SCHEMA notification TO notification_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA notification
GRANT ALL ON TABLES TO notification_user;

--retention_termination
GRANT USAGE ON SCHEMA retention_termination TO retention_termination_user;
GRANT CREATE ON SCHEMA retention_termination TO retention_termination_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA 
GRANT ALL ON TABLES TO retention_termination_user;

--sale_contracting
GRANT USAGE ON SCHEMA sale_contracting TO sale_contracting_user;
GRANT CREATE ON SCHEMA sale_contracting TO sale_contracting_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA sale_contracting
GRANT ALL ON TABLES TO sale_contracting_user;

--technical_support
GRANT USAGE ON SCHEMA technical_support TO technical_support_user;
GRANT CREATE ON SCHEMA technical_support TO technical_support_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA technical_support
GRANT ALL ON TABLES TO technical_support_user;