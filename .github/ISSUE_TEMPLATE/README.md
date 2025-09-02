# GitHub Issue Templates

This directory contains structured issue templates for the Axs Access Control Service project to help users provide comprehensive descriptions when reporting issues or requesting features.

## Available Templates

### 🐛 Bug Report (`bug_report.yml`)
Use this template to report bugs or unexpected behavior in the system.
- **Fields**: Description, reproduction steps, expected vs actual behavior, environment details
- **Labels**: `bug`, `triage`
- **Purpose**: Standardize bug reporting with all necessary technical details

### ✨ Feature Request (`feature_request.yml`)
Use this template to suggest new features or enhancements.
- **Fields**: Feature description, problem/solution, use cases, acceptance criteria
- **Labels**: `enhancement`, `feature-request`
- **Purpose**: Ensure feature requests include clear requirements and justification

### 📚 Documentation (`documentation.yml`)
Use this template to suggest improvements to documentation.
- **Fields**: Issue description, location, suggested improvements, target audience
- **Labels**: `documentation`, `improvement`
- **Purpose**: Help maintain high-quality, clear documentation

### ❓ Question/Help (`question.yml`)
Use this template when you need help or have questions about using the system.
- **Fields**: Question details, context, attempted solutions, environment
- **Labels**: `question`, `help-wanted`
- **Purpose**: Provide structured support requests with sufficient context

### 🔒 Security (`security.yml`)
Use this template for **non-critical** security improvements only.
- **Fields**: Security concern, severity, impact, mitigation suggestions
- **Labels**: `security`, `vulnerability`
- **Purpose**: Handle public security discussions (critical issues should use private reporting)

## Configuration

The `config.yml` file:
- Disables blank issues (all issues must use a template)
- Provides helpful contact links to documentation and CI status
- Guides users to relevant resources before creating issues

## Key Features

### Comprehensive Description Fields
All templates include:
- **Required description fields** that guide users to provide clear, detailed information
- **Structured sections** for different types of information (reproduction steps, environment, etc.)
- **Validation rules** to ensure critical information is provided
- **Pre-submission checklists** to verify completeness

### Component Classification
Templates help categorize issues by affected component:
- API (Service endpoints)
- Application Layer (Commands/Queries)
- Infrastructure (Database/Repositories)
- Domain (Entities/Business Logic)
- Documentation
- Build/CI

### Consistent Workflow
All templates:
- Auto-assign to project maintainer (`jkulba`)
- Apply appropriate labels for triage
- Include consistent formatting and structure
- Provide clear guidance on required vs optional fields

## Benefits

1. **Better Issue Quality**: Structured templates ensure issues contain necessary information
2. **Faster Triage**: Consistent labeling and categorization speeds up issue review
3. **Reduced Back-and-forth**: Comprehensive templates minimize need for additional information requests
4. **Clear Expectations**: Users understand what information is needed for different issue types
5. **Improved Documentation**: Dedicated template encourages documentation improvements

## Usage

When creating a new issue in GitHub:
1. Select the appropriate template from the dropdown
2. Fill out all required fields completely
3. Review the pre-submission checklist
4. Submit the issue

The templates will automatically apply appropriate labels and assignments for efficient project management.