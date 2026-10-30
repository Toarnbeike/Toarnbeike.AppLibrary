# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)
and this project adheres to [Semantic Versioning](https://semver.org/).

## [0.1.0]
Initial release (internal)

### Added
- `Core` library with:
	- Extensions for DateTime and formatting Type Names.
	- Application services `IDateTimeProvider` and `ICurrentUserProvider` with default implementations and default DI registration call.
	- Extensions for `OptionsBuilder<T>` to add data annotation validation for nested option values.
	- Extensions for `IServiceCollection` to replace all registrations and to register all implementations of a given interface type.
	- Application failures for unexpected situations.

- `Entities` library with:
	- Entity interfaces, with auditing support and strongly typed IDs, 
	- Transitions for immutable domain objects, with extensions.
	- Domain failures with extensions
	- DomainEvents

- `TicketApp` (example app) with:
	- Domain model for a ticketing system, with detailed transition logic and domain validation.

- Full test coverage for non-example code.