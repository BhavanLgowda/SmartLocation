
namespace SmartLocationApp.Source
{
  public enum ErrorCode : uint
  {
    Device_is_working_properly = 0,
    Device_is_not_configured_correctly = 1,
    Windows_cannot_load_the_driver_for_this_device = 2,
    Driver_for_this_device_might_be_corrupted_or_the_system_may_be_low_on_memory_or_other_resources = 3,
    Device_is_not_working_properly_One_of_its_drivers_or_the_registry_might_be_corrupted = 4,
    Driver_for_the_device_requires_a_resource_that_Windows_cannot_manage = 5,
    Boot_configuration_for_the_device_conflicts_with_other_devices = 6,
    Cannot_filter = 7,
    Driver_loader_for_the_device_is_missing = 8,
    Device_is_not_working_properly_The_controlling_firmware_is_incorrectly_reporting_the_resources_for_the_device = 9,
    Device_cannot_start = 10, // 0x0000000A
    Device_failed = 11, // 0x0000000B
    Device_cannot_find_enough_free_resources_to_use = 12, // 0x0000000C
    Windows_cannot_verify_the_device_resources = 13, // 0x0000000D
    Device_cannot_work_properly_until_the_computer_is_restarted = 14, // 0x0000000E
    Device_is_not_working_properly_due_to_a_possible_reenumeration_problem = 15, // 0x0000000F
    Windows_cannot_identify_all_of_the_resources_that_the_device_uses = 16, // 0x00000010
    Device_is_requesting_an_unknown_resource_type = 17, // 0x00000011
    Device_drivers_must_be_reinstalled = 18, // 0x00000012
    Failure_using_the_VxD_loader = 19, // 0x00000013
    Registry_might_be_corrupted = 20, // 0x00000014
    System_failure_If_changing_the_device_driver_is_ineffective_see_the_hardware_documentation_Windows_is_removing_the_device = 21, // 0x00000015
    Device_is_disabled = 22, // 0x00000016
    System_failure_If_changing_the_device_driver_is_ineffective_see_the_hardware_documentation = 23, // 0x00000017
    Device_is_not_present_not_working_properly_or_does_not_have_all_of_its_drivers_installed = 24, // 0x00000018
    Device_does_not_have_valid_log_configuration = 27, // 0x0000001B
    Windows_is_still_setting_up_the_device = 27, // 0x0000001B
    Device_drivers_are_not_installed = 28, // 0x0000001C
    Device_is_disabled_The_device_firmware_did_not_provide_the_required_resources = 29, // 0x0000001D
    Device_is_using_an_IRQ_resource_that_another_device_is_using = 30, // 0x0000001E
    Device_is_not_working_properly_Windows_cannot_load_the_required_device_drivers = 31, // 0x0000001F
  }
}
